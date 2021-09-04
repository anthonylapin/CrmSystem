import { Injectable, TemplateRef } from '@angular/core';
import { ObjectHelperUtil } from '../utilities/objectHelper.util';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  toasts: any[] = [];

  show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    if (ObjectHelperUtil.isEmpty(options)) {
      this.toasts.push({ textOrTpl, classname: 'bg-secondary text-light', delay: 3000, autohide: true });
    } else {
      this.toasts.push({ textOrTpl, ...options });
    }
  }

  showDanger(textOrTpl: string | TemplateRef<any>) {
    this.toasts.push({ textOrTpl, classname: 'bg-danger text-light', delay: 3000, autohide: true });
  }

  remove(toast: any) {
    this.toasts = this.toasts.filter((t) => t !== toast);
  }
}
