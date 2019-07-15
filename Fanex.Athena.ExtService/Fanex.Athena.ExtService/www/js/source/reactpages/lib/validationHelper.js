import AppData from './AppData';

export const validateBaseCurrency=(baseCurrency = "", allowEmpty = true)=> {
    return (allowEmpty && !baseCurrency) || (!!baseCurrency && AppData.baseCurrencyList.find(x => x.value === baseCurrency));
}