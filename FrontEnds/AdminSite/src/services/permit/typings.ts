declare namespace PermitApi {
  interface FuncItem extends BaseMo {
    status: number;
    code: string;
    parent_code: string;
    title: string;
  }

  interface FuncTreeItem extends FuncItem {
    children?: FuncTreeItem[];
  }

  interface ChangeFuncItemReq {
    title: string;
    parent_code?: string;
  }
  interface AddFuncItemReq extends ChangeFuncItemReq {
    code: string;
  }
}
