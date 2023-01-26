declare namespace ProductApi {
  interface SpuMo extends BaseMo {
    status: number;
    desp: string;
    title: string;
  }

  interface CategoryMo extends BaseMo {
    name: string;
    order: number;
    children: CategoryMo[];
  }

  interface AddCategoryReq extends UpdateCategoryReq {
    parent_id: string;
  }

  interface UpdateCategoryReq {
    name: string;
    order: number;
  }
}
