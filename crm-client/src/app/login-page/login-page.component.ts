import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../shared/services/auth.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { UiService } from '../shared/utilities/ui.service';
import { ToastService } from '../shared/services/toast.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
})
export class LoginPageComponent implements OnInit, OnDestroy {
  form: FormGroup;
  aSub: Subscription;
  afterRegistration: boolean = false;
  accessDenied: boolean = false;
  sessionExpired: boolean = false;

  constructor(
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required, Validators.minLength(6)]),
    });

    this.route.queryParams.subscribe((params: Params) => {
      this.afterRegistration = Boolean(params['registered']);
      this.accessDenied = Boolean(params['accessDenied']);
      this.sessionExpired = Boolean(params[params['sessionExpired']]);
    });
  }

  ngOnDestroy() {
    if (this.aSub) {
      this.aSub.unsubscribe();
    }
  }

  onSubmit() {
    this.form.disable();
    this.aSub = this.auth.login(this.form.value).subscribe(
      () => this.router.navigate(['/overview']),
      () => {
        this.toastService.showDanger('Invalid credentials');
        this.form.enable();
      }
    );
  }
}
