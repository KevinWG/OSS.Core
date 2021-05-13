import React from 'react';
import { Popconfirm, Tooltip, Button } from 'antd';
import { PopconfirmProps } from 'antd/lib/popconfirm';
import { ButtonProps } from 'antd/lib/button';
import FuncAccess from '../func_access';

interface FetchConfirmButtonProps extends ButtonProps {
  tip_title?: string;
  func_code?: string;
  confirm_props: PopconfirmProps;
}

const FetchConfirmButton: React.FC<FetchConfirmButtonProps> = (props) => {
  const { func_code, confirm_props, tip_title, ...restProps } = props;

  return (
    <FuncAccess func_code={func_code}>
      <Popconfirm {...confirm_props}>
        <Tooltip title={tip_title}>
          <Button {...restProps}>{props.children}</Button>
        </Tooltip>
      </Popconfirm>
    </FuncAccess>
  )
};


export default FetchConfirmButton;
