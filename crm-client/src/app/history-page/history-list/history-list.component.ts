import { Component, Input, OnInit } from '@angular/core';
import { IOrderViewModel } from '../../shared/interfaces';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-history-list',
  templateUrl: './history-list.component.html',
  styleUrls: ['./history-list.component.scss'],
})
export class HistoryListComponent implements OnInit {
  @Input() orders: IOrderViewModel[];
  selectedOrder: IOrderViewModel;

  constructor(private modalService: NgbModal) {}

  ngOnInit(): void {}

  onOpenModal(content: any, order: IOrderViewModel) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      () => {},
      () => {}
    );
    this.selectedOrder = order;
  }
}
