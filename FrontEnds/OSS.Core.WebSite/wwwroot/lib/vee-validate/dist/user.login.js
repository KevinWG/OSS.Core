import { Validator } from 'vee-validate';

const dictionary = {
    en: {
        attributes: {
            email: 'Email Address'
        }
    }
};

Validator.updateDictionary(dictionary);