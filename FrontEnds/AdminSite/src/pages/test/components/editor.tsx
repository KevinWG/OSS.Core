import { comSearchOrg } from '@/pages/portal/organization/service';
import { getAllWarehouse } from '@/pages/wms/warehouse/service';
import {
  DrawerForm,
  DrawerFormProps,
  ProFormDatePicker,
  ProFormGroup,
  ProFormSelect,
  ProFormText,
  ProFormTreeSelect,
} from '@ant-design/pro-components';
import { Form } from 'antd';
import moment from 'moment';
import { useEffect } from 'react';
import { add, edit, StockDirection } from '../service';

interface ApplyEditorProps extends IEditorProps<WmsApi.StockApplyMo>, DrawerFormProps {}
export default function ApplyEditor({ record, callback, ...restProps }: ApplyEditorProps) {
  const form = Form.useForm()[0];

  useEffect(() => {
    if (record.id != '-1') form.resetFields();
  }, [record]);

  return (
    <DrawerForm
      title="添加申请"
      form={form}
      onFinish={async (values: any) => {
        values.plan_time = moment(values.plan_time).unix();
        values.direction = parseInt(values.direction);

        const isAdd = record.id == '0';

        return (isAdd ? add(values) : edit(record['_pass_token_id'], values)).then((res) => {
          if (res.success) {
            form.resetFields();
          }
          callback(res);
        });
      }}
      initialValues={record}
      open={record.id != '-1'}
      {...restProps}
    >
      <ProFormGroup>
        <ProFormSelect
          width="md"
          name="direction"
          label="申请类型"
          placeholder="请选择申请类型"
          rules={[
            {
              required: true,
              message: '申请类型不能为空',
            },
          ]}
          convertValue={(v) => v?.toString()}
          valueEnum={StockDirection}
        />
        <ProFormTreeSelect
          label="仓库"
          width="md"
          name="warehouse_id"
          colProps={{ md: 12 }}
          request={async () =>
            getAllWarehouse(false).then((res) => {
              if (res.success) return res.data;
              return [];
            })
          }
          rules={[
            {
              required: true,
              message: '仓库不能为空',
            },
          ]}
          fieldProps={{
            fieldNames: {
              label: 'name',
              value: 'id',
            },
            treeDefaultExpandAll: true,
            placeholder: '请选择仓库',
          }}
        />
      </ProFormGroup>

      <ProFormGroup>
        <ProFormText
          width="md"
          name="title"
          label="标题"
          placeholder="请输入自定义标题"
          rules={[
            {
              type: 'string',
              max: 200,
              message: '标题不能超过200个字符',
            },
          ]}
        />
        <ProFormDatePicker
          name="plan_time"
          label="计划出入库时间"
          width="md"
          convertValue={(v) =>
            v > 0 ? moment(v * 1000).format('YYYY-MM-DD') : moment().format('YYYY-MM-DD')
          }
          rules={[
            {
              required: true,
              message: '计划出入库时间不能为空',
            },
          ]}
        />
      </ProFormGroup>

      <ProFormSelect
        width="md"
        name="org_id"
        label="收发组织对象"
        placeholder="请选择组织对象"
        fieldProps={{
          showSearch: true,
        }}
        rules={[
          {
            required: true,
            message: '组织对象不能为空',
          },
        ]}
        debounceTime={500}
        request={async (paras) => {
          const sReq = {
            filter: {
              // org_type: '100',
              name: paras.keyWords,
            },
          };
          return comSearchOrg(sReq as any).then((res) => {
            if (!res.success) return [];
            return res.data.map((o) => ({ label: o.name, value: o.id }));
          });
        }}
      />
    </DrawerForm>
  );
}
