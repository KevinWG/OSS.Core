import { Button, ButtonProps } from 'antd';
import FuncAccess from '../func_access';

interface FuncAccessButtonProps extends ButtonProps {
  func_code: string;
  hidden?: boolean;
}

export default function AccessButton(props: FuncAccessButtonProps) {
  const { func_code, hidden, ...restProps } = props;

  return (
    <FuncAccess hidden={hidden} func_code={func_code}>
      <Button {...restProps}>{props.children}</Button>
    </FuncAccess>
  );
}
