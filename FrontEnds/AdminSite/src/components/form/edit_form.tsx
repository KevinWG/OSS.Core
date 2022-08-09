import React from 'react';
import { Form, Row, Col } from 'antd';
import FormItemFactory, { FormItemFactoryProps } from './form_item_factory';
import { FormProps } from 'antd/lib/form';

export interface EditFormProps extends FormProps {
  items: FormItemFactoryProps[];
  row_item_count?: number;
}

/**
 * 编辑表单
 * 子组件（children） 会替代默认按钮区
 * @param props
 */
const EditForm: React.FC<EditFormProps> = (props) => {
  const { items, row_item_count, ...restProps } = props;

  const count = items.length;
  const colSpan = row_item_count
    ? { xs: 24 / row_item_count, sm: 24 / row_item_count }
    : {
        xs: 24,
        sm: count < 2 ? 24 : 12,
        md: count < 3 ? 24 / count : 8,
      };

  return (
    <Form {...restProps}>
      <Row>
        {items.map((item, index) => {
          return (
            <Col
              {...(item.col_span ? { span: item.col_span } : colSpan)}
              key={restProps.name + 'col_' + index}
              style={{ paddingRight: 15, marginBottom: 10 }}
            >
              <FormItemFactory {...item} />
            </Col>
          );
        })}
      </Row>
      <Row>
        <Col span={24} style={{ textAlign: 'right' }}>
          {props.children}
        </Col>
      </Row>
    </Form>
  );
};

export default EditForm;
