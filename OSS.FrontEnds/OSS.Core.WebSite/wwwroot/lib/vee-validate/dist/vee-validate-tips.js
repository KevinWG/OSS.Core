if (!!VeeValidate) {
    // 错误提示信息
    const dict = {
        en: {
            messages: {
                required: (field) => field + ' 不能为空！',
                min: (field, e) => field + " 不能少于" + e[0] + "位！"
}
        }
    };
    VeeValidate.Validator.updateDictionary(dict);
}
