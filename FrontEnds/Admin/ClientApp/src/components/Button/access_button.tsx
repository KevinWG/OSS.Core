import { useAccess, Access } from 'umi';
import React from 'react';
import { Popconfirm, Tooltip, Button } from 'antd';
import { PopconfirmProps } from 'antd/lib/popconfirm';
import { ButtonProps } from 'antd/lib/button';

interface AccessButtonProps extends ButtonProps {
  tip_title?: string;
  func_code?: string;

  confirm_props?: PopconfirmProps;
}

const AccessButton: React.FC<AccessButtonProps> = (props) => {
  const { func_code, ...restProps } = props;

  if (!func_code) {
    return <AccessButtonCheckConfirm {...restProps}>{props.children}</AccessButtonCheckConfirm>;
  }

  const access = useAccess();
  return (
    <>
      <Access accessible={access[func_code]}>
        <AccessButtonCheckConfirm {...restProps}>{props.children}</AccessButtonCheckConfirm>
      </Access>
    </>
  );
};

const AccessButtonCheckConfirm: React.FC<AccessButtonProps> = (props) => {
  const { confirm_props, tip_title, ...restProps } = props;
  if (!confirm_props) {
    return (
      <Tooltip title={tip_title}>
        <Button {...restProps}>{props.children}</Button>
      </Tooltip>
    );
  }

  return (
    <Popconfirm {...confirm_props}>
      <Tooltip title={tip_title}>
        <Button {...restProps}>{props.children}</Button>
      </Tooltip>
    </Popconfirm>
  );
};

export default AccessButton;
