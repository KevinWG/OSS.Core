import { Button, ButtonProps } from "antd";
import React from "react";

interface TableButtonProps extends ButtonProps {
    hidden?: boolean;
}

export default function TableButton(props: TableButtonProps) {
    const {  hidden, ...restProps } = props;

    return (
            <Button type="dashed" shape='round' size='small' {...restProps}>{props.children}</Button>    );
};