import AccessButton from '@/components/button/access_button';
import SearchProTable from '@/components/search/search_protable';
import FuncCodes from '@/services/common/func_codes';
import {
  ActionType,
  ModalForm,
  PageContainer,
  ProColumns,
  ProFormText,
} from '@ant-design/pro-components';
import { Form } from 'antd';
import { useRef, useState } from 'react';
import { addUnit, getAllUnits } from './service';

export default function UnitList() {
  const tableColumns: ProColumns<WmsApi.UnitMo>[] = [
    {
      title: '名称',
      dataIndex: 'name',
    },
  ];

  const defaultUnit = { id: '-1' } as WmsApi.UnitMo;
  const [editUnit, setEditUnit] = useState(defaultUnit);

  const tableRef = useRef<ActionType>();

  const form = Form.useForm()[0];

  return (
    <PageContainer>
      <SearchProTable<WmsApi.UnitMo>
        rowKey="id"
        search={false}
        pagination={false}
        // size="small"
        columns={tableColumns}
        actionRef={tableRef}
        request={getAllUnits}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.wms_unit_manage}
              onClick={() => {
                setEditUnit({ id: '0' } as any);
              }}
            >
              添加单位
            </AccessButton>,
          ],
        }}
      />
      <ModalForm
        title="添加单位"
        form={form}
        onFinish={async (values: any) => {
          return addUnit(values).then((res) => {
            if (res.success) {
              tableRef.current?.reload();
              setEditUnit(defaultUnit);
              form.resetFields();
            }
          });
        }}
        modalProps={{
          onCancel: () => {
            setEditUnit(defaultUnit);
          },
        }}
        open={editUnit.id != '-1'}
      >
        <ProFormText
          width="md"
          name="name"
          label="单位名称"
          placeholder="请输入单位名称"
          rules={[
            {
              required: true,
              type: 'string',
              max: 30,
              message: '单位名称必填且不能超过30个字符',
            },
          ]}
        />
      </ModalForm>
    </PageContainer>
  );
}
