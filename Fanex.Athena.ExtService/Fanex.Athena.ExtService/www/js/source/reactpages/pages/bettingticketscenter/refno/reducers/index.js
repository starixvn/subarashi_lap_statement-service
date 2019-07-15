import { combineReducers } from 'redux';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as usernameReducer from '@jsroot/reducers/usernameReducer'
import * as baseCurrencyReducer from '@jsroot/reducers/baseCurrencyReducer'
import * as loaderReducer from '@jsroot/reducers/loaderReducer'
import * as convertAllReducer from '@jsroot/reducers/convertAllReducer'
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer'
import * as reportTitleReducer from '@jsroot/reducers/reportTitleReducer'
import * as userGuideReducer from '@jsroot/reducers/userGuideReducer'

export default combineReducers(Object.assign(
    gridDataReducer,
    filterHandlerReducer,
    usernameReducer,
    baseCurrencyReducer,
    loaderReducer,
    convertAllReducer,
    requestParamReducer,
    reportTitleReducer,
    userGuideReducer
));