import React from 'react';
import { Form, Input, InputNumber,DatePicker, Select } from 'antd';
import { FormItemProps } from 'antd/lib/form';
import { SizeType } from 'antd/lib/config-provider/SizeContext';


const { RangePicker } = DatePicker;

//  添加可自定义Item类型
export enum FormItemFactoryType {
  input = 0,
  input_textarea = 1,
  input_number = 2,

  select = 10,


  data_picker=20,
  data_rangepicker=21,

  /**
   * 用户自定义
   */
  customer = 1000,
}

export interface FormItemFactoryProps extends FormItemProps {
  type: FormItemFactoryType;
  name: string;
  size?: SizeType;

  col_span?: number;
  custom_ele?: React.ReactNode;

  /**
   * item 下元素（如：Input）的扩展属性
   */
  ele_ext_props?: any
}

export default function FormItemFactory(props: FormItemFactoryProps) {
  const { type, name, size, custom_ele, ele_ext_props, ...restProps } = props;

  return (
    <Form.Item label={props.label} colon={true} key={'item_' + name} name={name} {...restProps}>
      {
        // 输入框处理
        (type == FormItemFactoryType.input && (
          <Input placeholder={'输入' + props.label} size={size} {...ele_ext_props} />
        )) ||
        (type == FormItemFactoryType.select && (
          <Select size={size}  {...ele_ext_props} />
        )) ||
        (type == FormItemFactoryType.input_textarea && (
          <Input.TextArea size={size} {...ele_ext_props} />
        )) ||
        (type == FormItemFactoryType.input_number && (
          <InputNumber size={size} {...ele_ext_props} />
        )) ||
        (type == FormItemFactoryType.data_picker && (
          <DatePicker  size={size}  {...ele_ext_props}/>
        )) ||
        (type == FormItemFactoryType.data_rangepicker && (
          <RangePicker size={size}  {...ele_ext_props}/>
        )) ||
        // 自定义控件
        (type == FormItemFactoryType.customer && custom_ele)
      }
    </Form.Item>
  );
}
