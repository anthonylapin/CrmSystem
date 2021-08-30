import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { PositionsService } from '../../../shared/services/positions.service';
import { IPosition } from '../../../shared/interfaces';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UiService } from '../../../shared/utilities/ui.service';

const initialModifyPositionFormState = {
  name: null,
  cost: null,
};

@Component({
  selector: 'app-positions-form',
  templateUrl: './positions-form.component.html',
  styleUrls: ['./positions-form.component.scss'],
})
export class PositionsFormComponent implements OnInit, OnDestroy {
  @Input('categoryId') categoryId: number;
  positions: IPosition[] = [];
  loading: boolean = false;
  form: FormGroup;
  positionId: number | null = null;

  constructor(private positionsService: PositionsService, private modalService: NgbModal) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(initialModifyPositionFormState.name, Validators.required),
      cost: new FormControl(initialModifyPositionFormState.cost, [Validators.required, Validators.min(1)]),
    });

    this.loading = true;
    this.positionsService.getAll(this.categoryId).subscribe((positions) => {
      this.positions = positions;
      this.loading = false;
    });
  }

  ngOnDestroy(): void {
    this.modalService.dismissAll();
  }

  private resetPositionForm() {
    this.form.reset({
      name: initialModifyPositionFormState.name,
      cost: initialModifyPositionFormState.cost,
    });
  }

  onOpenModal(content: any, position?: IPosition) {
    if (position) {
      this.form.patchValue({
        name: position.name,
        cost: position.cost,
      });
      this.positionId = position.id ?? null;
    } else {
      this.resetPositionForm();
      this.positionId = null;
    }

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      () => {},
      () => {}
    );
  }

  onDeletePosition(event: Event, position: IPosition) {
    event.stopPropagation();
    if (UiService.confirmMessage(`Are you sure you want to delete this position: ${position.name}?`)) {
      this.positionsService.delete(position).subscribe(
        () => {
          const index = this.positions.findIndex((p) => p.id === position.id);
          this.positions.splice(index, 1);
        },
        (error) => {
          UiService.alertMessage(error.error.message);
        }
      );
    }
  }

  private onModifyPositionErrorCallback(error: any) {
    UiService.alertMessage(error.error.message);
  }

  private onCreatePosition(position: IPosition) {
    this.positionsService.create(position).subscribe(
      (newPosition) => {
        UiService.alertMessage('Position was created');
        this.positions.push(newPosition);
      },
      this.onModifyPositionErrorCallback,
      () => {
        this.modalService.dismissAll();
        this.resetPositionForm();
        this.form.enable();
      }
    );
  }

  private onUpdatePosition(position: IPosition) {
    this.positionsService.update(position).subscribe(
      (newPosition) => {
        const index = this.positions.findIndex((p) => p.id === position.id);
        this.positions[index] = newPosition;
        UiService.alertMessage('Position was updated.');
      },
      this.onModifyPositionErrorCallback,
      () => {
        this.modalService.dismissAll();
        this.resetPositionForm();
        this.form.enable();
      }
    );
  }

  onSubmitPosition() {
    this.form.disable();

    const position: IPosition = {
      name: this.form.value.name,
      cost: this.form.value.cost,
      categoryId: this.categoryId,
    };

    if (this.positionId) {
      position.id = this.positionId;
      this.onUpdatePosition(position);
    } else {
      this.onCreatePosition(position);
    }
  }
}
