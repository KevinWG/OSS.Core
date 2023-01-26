import {
  DrawerForm,
  DrawerFormProps,
  ProForm,
  ProFormDigit,
  ProFormInstance,
  ProFormList,
  ProFormSelect,
  ProFormText,
  ProFormTextArea,
  ProFormTreeSelect,
} from '@ant-design/pro-components';
import { useEffect, useRef } from 'react';
import { category_search } from '../../mcategory/service';
import { getAllUnits } from '../../unit/service';
import { addMaterial, MaterialType, updateMaterial } from '../service';

interface MaterialEditorProps extends DrawerFormProps, IEditorProps<WmsApi.Material> {}

export default function MaterialEditor({ callback, record, ...restProps }: MaterialEditorProps) {
  const formRef = useRef<ProFormInstance>();

  useEffect(() => {
    if (record.id != '-1') formRef.current?.resetFields();
  }, [record]);

  return (
    <DrawerForm
      title="添加修改物料"
      formRef={formRef}
      grid={true}
      autoFocusFirstInput
      onFinish={async (values: any) => {
        const isAdd = record.id == '0';
        values.type = parseInt(values.type);
        return (isAdd ? addMaterial(values) : updateMaterial(record.id, values)).then((res) => {
          callback(res);
          return res.success;
        });
      }}
      initialValues={record}
      {...restProps}
    >
      <ProForm.Group>
        <ProFormText
          name="name"
          label="名称"
          colProps={{ md: 12 }}
          rules={[
            {
              required: true,
              type: 'string',
              max: 80,
              message: '请输入名称（不能超过80个字符）',
            },
          ]}
          tooltip="物料名称"
          placeholder="请输入名称"
        />
        <ProFormText
          name="code"
          disabled={record.id != '0'}
          label="物料编码"
          colProps={{ md: 6 }}
          rules={[
            {
              type: 'string',
              max: 80,
              message: '请输入名称（不能超过80个字符）',
            },
          ]}
          tooltip="物料编码唯一且不可修改，如果不填系统默认填充"
          placeholder="请输入物料编码"
        />
        <ProFormTreeSelect
          label="分类"
          name="c_id"
          colProps={{ md: 6 }}
          request={async () =>
            category_search().then((res) => {
              if (res.success) return res.data;
              return [];
            })
          }
          fieldProps={{
            fieldNames: {
              label: 'name',
              value: 'id',
            },
            treeDefaultExpandAll: true,
            placeholder: '请选择物料分组',
          }}
        />
      </ProForm.Group>

      <ProForm.Group>
        <ProFormSelect
          name="type"
          colProps={{ md: 6 }}
          label="物料类型"
          convertValue={(v) => v?.toString()}
          valueEnum={MaterialType}
        />
        <ProFormText
          colProps={{ md: 18 }}
          name="tec_spec"
          // width="lg"
          label="规格参数"
          rules={[
            {
              type: 'string',
              max: 200,
              message: '不能超过200个字符',
            },
          ]}
          placeholder="物料规格参数描述"
        />
      </ProForm.Group>
      <ProForm.Group>
        <ProFormSelect
          disabled={record.id != '0'}
          name="basic_unit"
          colProps={{ md: 6 }}
          rules={[
            {
              required: true,
              type: 'string',
              message: '标准（最小）库存单位不能为空',
            },
          ]}
          label="标准（最小）库存单位"
          tooltip="库存使用此单位转换，创建后不可修改!"
          request={async () => getAllUnits().then((res) => (res.success ? res.data : []))}
          fieldProps={{
            fieldNames: {
              value: 'name',
              label: 'name',
            },
          }}
        />
        <ProFormList name="multi_units" colProps={{ md: 18 }}>
          {() => {
            return (
              <ProForm.Group>
                <ProFormSelect
                  name="unit"
                  colProps={{ md: 12 }}
                  label="目标单位"
                  request={async () => getAllUnits().then((res) => (res.success ? res.data : []))}
                  fieldProps={{
                    fieldNames: {
                      value: 'name',
                      label: 'name',
                    },
                  }}
                  rules={[
                    {
                      required: true,
                      type: 'string',
                      message: '目标单位不能为空',
                    },
                  ]}
                />
                <ProFormDigit
                  label="系数"
                  colProps={{ md: 12 }}
                  name="ratio"
                  width={'xs'}
                  addonBefore="="
                  addonAfter={formRef.current?.getFieldValue('basic_unit')}
                />
              </ProForm.Group>
            );
          }}
        </ProFormList>
      </ProForm.Group>

      <ProFormTextArea name="remark" label="备注信息" placeholder="备注信息" />
    </DrawerForm>
  );
}
