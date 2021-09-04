export class UiService {
  static confirmMessage(message: string): boolean {
    return confirm(message);
  }
}
