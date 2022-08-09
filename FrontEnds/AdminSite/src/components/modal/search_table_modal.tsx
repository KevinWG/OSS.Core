import { ParamsType } from '@ant-design/pro-components';
import { Button, Modal, ModalProps } from 'antd';
import SearchProTable, { SearchProTableProps } from '../search/search_protable';

export interface SelectTabelModalProps<T extends BaseMo, U, ValueType>
  extends SearchProTableProps<T, U, ValueType> {
  selected: (t: T) => void;
  modal_props: ModalProps;
}

export default function SelectTabelModal<
  T extends BaseMo,
  Params extends ParamsType = ParamsType,
  ValueType = 'text',
>(props: SelectTabelModalProps<T, Params, ValueType>) {
  const { selected, columns, modal_props, ...restProps } = props;

  const newCols = columns
    ? [
        ...columns,
        {
          title: '操作',
          dataIndex: 'id',
          hideInSearch: true,
          render: (v: any, r: T) => (
            <Button
              type="primary"
              onClick={() => {
                selected(r);
              }}
            >
              选择
            </Button>
          ),
        },
      ]
    : [];

  return (
    <Modal footer={false} {...modal_props}>
      <SearchProTable<T, Params, ValueType>
        rowKey="id"
        size="small"
        search={{ span: 12, defaultCollapsed: true }}
        columns={newCols}
        pagination={{ pageSize: 10 }}
        {...restProps}
      />
    </Modal>
  );
}
