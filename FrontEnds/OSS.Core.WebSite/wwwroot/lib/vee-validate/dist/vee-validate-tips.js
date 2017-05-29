if (!!VeeValidate) {
    // 错误提示信息
    const dict = {
        en: {
            messages: {
                required: (field) => field + ' 不能为空'
            }
        }
    };
    VeeValidate.Validator.updateDictionary(dict);
}
