import AccessButton from '@/components/button/access_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import {
  ActionType,
  ModalForm,
  ModalFormProps,
  PageContainer,
  ProColumns,
  ProFormGroup,
  ProFormSelect,
  ProFormText,
} from '@ant-design/pro-components';
import { Form } from 'antd';
import moment from 'moment';
import { useRef, useState } from 'react';
import { addOrg, OrgType, searchOrg, setOrgUabale } from './service';

export default function OrgList() {
  const statusButtons = [
    {
      condition: (r: PortalApi.Orgnization) => r.status == 0,
      buttons: [
        {
          title: '禁用',

          fetch: (item: PortalApi.Orgnization) => setOrgUabale(item['_pass_token_id'], 0),
          fetch_desp: '禁用当前往来对象',
          func_code: FuncCodes.portal_org_manage,
          icon: <LockOutlined />,
        },
      ],
    },
    {
      condition: (r: PortalApi.Orgnization) => r.status == -100,
      buttons: [
        {
          title: '解禁',

          fetch: (item: PortalApi.Orgnization) => setOrgUabale(item['_pass_token_id'], 1),
          fetch_desp: '解禁当前往来对象',
          func_code: FuncCodes.portal_org_manage,
          icon: <UnlockOutlined />,
        },
      ],
    },
  ];
  const tableColumns: ProColumns<PortalApi.Orgnization>[] = [
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '合作类型',
      dataIndex: 'org_type',
      valueType: 'select',
      valueEnum: OrgType,
    },
    {
      title: '创建时间',
      dataIndex: 'add_time',
      hideInSearch: true,
      render: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
    },
    {
      title: '状态',
      dataIndex: 'status',
      valueType: 'select',
      valueEnum: CommonStatus,
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: PortalApi.Orgnization) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) tableRef.current?.reload();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  const defaultItem = { id: '-1' } as PortalApi.Orgnization;
  const [editItem, setEditItem] = useState(defaultItem);

  const tableRef = useRef<ActionType>();

  return (
    <PageContainer>
      <SearchProTable<PortalApi.Orgnization>
        rowKey="id"
        columns={tableColumns}
        actionRef={tableRef}
        request={searchOrg}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              //   size="small"
              func_code={FuncCodes.portal_org}
              onClick={() => {
                setEditItem({ id: '0' } as any);
              }}
            >
              添加往来对象
            </AccessButton>,
          ],
        }}
      />
      <OrgEditor
        record={editItem}
        callback={(res) => {
          if (res.success) {
            setEditItem(defaultItem);
            tableRef.current?.reload();
          }
        }}
        modalProps={{
          onCancel: () => {
            setEditItem(defaultItem);
          },
        }}
      ></OrgEditor>
    </PageContainer>
  );
}
interface OrgEditorProps extends IEditorProps<PortalApi.Orgnization>, ModalFormProps {}

function OrgEditor({ record, callback, ...restProps }: OrgEditorProps) {
  const form = Form.useForm()[0];

  return (
    <ModalForm
      title="添加往来对象"
      form={form}
      onFinish={async (values: any) => {
        values.org_type = parseInt(values.org_type);
        return addOrg(values).then((res) => {
          if (res.success) {
            form.resetFields();
          }
          callback(res);
        });
      }}
      open={record.id != '-1'}
      {...restProps}
    >
      <ProFormGroup>
        <ProFormSelect
          width="md"
          name="org_type"
          label="往来对象类型"
          placeholder="请选择对象类型"
          rules={[
            {
              required: true,
              message: '对象类型不能为空',
            },
          ]}
          convertValue={(v) => v?.toString()}
          valueEnum={OrgType}
        />
      </ProFormGroup>

      <ProFormGroup>
        <ProFormText
          width="md"
          name="name"
          label="公司/组织名称"
          placeholder="请输入公司/组织名称"
          rules={[
            {
              type: 'string',
              max: 300,
              message: '名称不能超过300个字符',
            },
          ]}
        />
      </ProFormGroup>
    </ModalForm>
  );
}
