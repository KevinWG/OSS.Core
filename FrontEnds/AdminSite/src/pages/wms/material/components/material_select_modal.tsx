import SelectTableModalElement from '@/components/form/select_table_modal_element';
import { searchMaterials } from '../service';

export default function MaterialSelectModal(props: {
  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  value?: any;

  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  onChange?: (v: any) => void;
}) {
  const cols = [
    {
      title: '物料编码',
      dataIndex: 'code',
    },
    {
      title: '物料名称',
      dataIndex: 'name',
    },
  ];
  return (
    <SelectTableModalElement<WmsApi.Material>
      display={(u) => {
        if (u)
          return (
            <div style={{ marginTop: 15, textAlign: 'center' }}>
              <div style={{ marginTop: 10 }}>{'(' + u.code + ') ' + u.name}</div>
            </div>
          );
        return undefined;
      }}
      params={{ status: '0' }}
      columns={cols}
      request={searchMaterials}
      value_change_transform={(u) => u.id}
      {...props}
    ></SelectTableModalElement>
  );
}
