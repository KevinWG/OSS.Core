import React from 'react';
import { Popconfirm,  Button } from 'antd';
import { PopconfirmProps } from 'antd/lib/popconfirm';
import { ButtonProps } from 'antd/lib/button';
import FuncAccess from '../func_access';

interface FetchConfirmButtonProps extends ButtonProps {
  func_code?: string;
  confirm_props: PopconfirmProps;
}

const FetchConfirmButton: React.FC<FetchConfirmButtonProps> = (props) => {
  const { func_code, confirm_props, ...restProps } = props;
  return confirm_props ? (
    <FuncAccess func_code={func_code}>
      <Popconfirm {...confirm_props}>
        <Button {...restProps}>{props.children}</Button>
      </Popconfirm>
    </FuncAccess>
  )
  : (<FuncAccess func_code={func_code}>
    <Button {...restProps}>{props.children}</Button>
  </FuncAccess>)
};


export default FetchConfirmButton;
