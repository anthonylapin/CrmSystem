export class ObjectHelperUtil {
  public static isEmpty(obj: any) {
    return obj && Object.keys(obj).length === 0 && obj.constructor === Object;
  }
}
