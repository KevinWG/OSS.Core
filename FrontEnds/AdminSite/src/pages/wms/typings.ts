declare namespace WmsApi {
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

  interface AddMaterialReq {
    name: string;
    code: string;
    c_id: string;
    type: number;
    basic_unit: string;
    factory_serial: string;
    tec_spec: string;
  }

  interface UnitMo extends BaseMo {
    name: string;
  }

  interface Material extends BaseMo, AddMaterialReq {
    multi_units: MaterialMultiUnitItem[];
  }

  interface MaterialMultiUnitItem {
    /// <summary>
    ///  单位
    /// </summary>
    unit: string;

    /// <summary>
    ///  相对基础单位系数
    /// </summary>
    ratio: number;
  }

  interface MSku extends BaseMo, AddMSkuReq {}

  interface AddMSkuReq {
    unit: string;
    ratio: number;
    bar_code: string;
  }

  interface UpdateWarehouseReq {
    name: string;
    remark: string;
  }

  interface AddWarehouseReq extends UpdateWarehouseReq {
    parent_id: string;
  }

  interface Warehouse extends BaseMo {
    name: string;
    parent_name: string;
    remark: string;
    parent_id: string;
    children: Warehouse[];
  }

  interface UpdateWareAreaReq {
    remark: string;
  }

  interface AddWareAreaReq extends UpdateWareAreaReq {
    warehouse_id: string;
    code: string;
  }

  interface WareArea extends BaseMo {
    code: string;
    remark: string;
    trade_flag: number;
  }

  interface AddStockApplyReq {
    title: string;
    warehouse_id: string;
    company_id: string;
    plan_time: number;
  }

  interface StockApplyMo extends BaseMo, AddStockApplyReq {
    warehouse_name: string;
    company_name: string;
    direction: number;
  }

  interface BatchMo extends BaseMo {
    code: string;
    material_id: number;
    material_code: string;
    expire_date: number;
  }

  interface AddBatchReq {
    code: string;
    material_id: number;
    expire_date: number;
  }
}
