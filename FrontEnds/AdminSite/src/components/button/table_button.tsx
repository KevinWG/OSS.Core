import { ButtonProps } from 'antd';
import AccessButton from './access_button';

interface TableButtonProps extends ButtonProps {
  func_code?: string;
}

export default function TableButton(props: TableButtonProps) {
  const { func_code, ...restProps } = props;

  return (
    <AccessButton
      type="dashed"
      shape="round"
      size="small"
      func_code={func_code || ''}
      {...restProps}
    ></AccessButton>
  );
}
