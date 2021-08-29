import { ElementRef } from '@angular/core';

export class UiService {
  // replace with some toast message in future
  static alertMessage(message: string) {
    alert(message);
  }

  static confirmMessage(message: string): boolean {
    return confirm(message);
  }

  static initializeFloatingButton(ref: ElementRef) {}
}
