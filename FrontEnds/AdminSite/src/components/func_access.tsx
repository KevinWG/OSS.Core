import React from "react";
import { Access, useAccess } from "umi";

interface FuncAccessProps {
    func_code?: string;
    children?: React.ReactNode|React.ReactNodeArray;
    hidden?: boolean;
}

export default function FuncAccess(props: FuncAccessProps) {
    const { func_code,hidden,children } = props;

    if (hidden) {
        return (<></>);
    }

    if (!func_code) {
        return (<>{children}</>);
    }

    const access = useAccess();
    return (
        <Access accessible={access[func_code]}>
            {children}
        </Access>
    );
};