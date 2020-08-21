import React from 'react';
import { Form, Input } from 'antd';
import { FormItemProps } from 'antd/lib/form';
import { SizeType } from 'antd/lib/config-provider/SizeContext';

//  添加可自定义Item类型
export enum FormItemFactoryType {
  input = 0,
  /**
   * 用户自定义
   */
  customer = 1000,
}

export interface FormItemFactoryProps extends FormItemProps {
  type: FormItemFactoryType;
  name: string;
  size?: SizeType;
  custom_ele?: React.ReactNode;
  col_span?: number;
  // extra?:{}
}

export default function FormItemFactory(props: FormItemFactoryProps) {
  const { type, name, size, custom_ele, ...restProps } = props;

  return (
    <Form.Item label={props.label} colon={true} key={'item_' + name} name={name} {...restProps}>
      {
        // 输入框处理
        (type == FormItemFactoryType.input && (
          <Input placeholder={'输入' + props.label} size={size} />
        )) ||
          // 自定义控件
          (type == FormItemFactoryType.customer && custom_ele)
      }
    </Form.Item>
  );
}
