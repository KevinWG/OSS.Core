import {
  DrawerForm,
  DrawerFormProps,
  ProForm,
  ProFormDatePicker,
  ProFormGroup,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import moment from 'moment';
import MaterialSelectModal from '../../material/components/material_select_modal';
import { addBatch } from '../service';

interface AddBatchProps extends IEditorProps<WmsApi.BatchMo>, DrawerFormProps {}

export default function AddBatch({ record, callback, ...restProps }: AddBatchProps) {
  return (
    <DrawerForm
      open={record.id != '-1'}
      {...restProps}
      onFinish={async (values: any) => {
        // console.info(values);
        values.expire_date = moment(values.expire_date).unix();
        return addBatch(values).then((res) => {
          callback(res);
          return res.success;
        });
      }}
    >
      <ProForm.Item name="material_id" label="物料信息：">
        <MaterialSelectModal></MaterialSelectModal>
      </ProForm.Item>

      <ProFormGroup>
        <ProFormText
          name="code"
          disabled={record.id != '0'}
          label="批次号"
          width="md"
          rules={[
            {
              type: 'string',
              max: 80,
              message: '请输入批次号（不能超过80个字符）',
            },
          ]}
          tooltip="批次号唯一且不可修改，如果不填系统默认填充"
          placeholder="请输入批次号编码"
        />
        <ProFormDatePicker
          name="expire_date"
          label="有效期"
          width="md"
          rules={[
            {
              required: true,
              message: '有效期不能为空',
            },
          ]}
        />
      </ProFormGroup>

      <ProFormTextArea name="remark" label="备注信息" placeholder="备注信息" />
    </DrawerForm>
  );
}
