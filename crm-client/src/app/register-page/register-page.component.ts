import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../shared/services/auth.service';
import { UiService } from '../shared/utilities/ui.service';
import { ToastService } from '../shared/services/toast.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
})
export class RegisterPageComponent implements OnInit, OnDestroy {
  form: FormGroup;
  aSub: Subscription;

  constructor(private authService: AuthService, private router: Router, private toastService: ToastService) {}

  ngOnDestroy(): void {
    if (this.aSub) {
      this.aSub.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required, Validators.minLength(6)]),
    });
  }

  onSubmit() {
    this.form.disable();
    this.aSub = this.authService.register(this.form.value).subscribe(
      () => {
        this.router.navigate(['/login'], {
          queryParams: {
            registered: true,
          },
        });
      },
      (error) => {
        const errMessage = Object.entries(error.error).reduce((acc, [k, v]) => {
          return `${acc} ${(v as string[])[0]}`;
        }, '');
        this.toastService.showDanger(errMessage ? errMessage : 'Something went wrong.');
        this.form.enable();
      }
    );
  }
}
