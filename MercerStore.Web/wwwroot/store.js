import { configureStore } from '@reduxjs/toolkit';
import cartReducer from './features/cart/redux/cartSlice';
import categoryReducer from './features/categoryPage/redux/categorySlice';
import searchPageReducer from './features/searchPage/redux/searchPageSlice';
import reviewsReducer from './features/reviews/redux/reviewSlice';
import adminSearchPageReducer from './features/Admin/searchPage/redux/adminSearchPageSlice';
import adminProductPageReducer from './features/Admin/productPage/redux/adminProductPageSlice';
import orderPageReducer from './features/Admin/orderPage/redux/orderPageSlice';
import userPageReducer from './features/Admin/userPage/redux/userPageSlice';
import reviewPageReducer from './features/Admin/reviewPage/redux/reviewPageSlice';
import supplierPageReducer from './features/Admin/supplierPage/redux/supplierPageSlice';
import invoicePageReducer from './features/Admin/invoicePage/redux/invoicePageSlice';
import metricPageReducer from './features/Admin/metricPage/redux/metricPageSlice';

const store = configureStore({
    reducer: {
        cart: cartReducer,
        category: categoryReducer,
        searchPage: searchPageReducer,
        reviews: reviewsReducer,
        adminSearchPage: adminSearchPageReducer,
        adminProductPage: adminProductPageReducer,
        orderPage: orderPageReducer,
        userPage: userPageReducer,
        reviewPage: reviewPageReducer,
        supplierPage: supplierPageReducer,
        invoicePage: invoicePageReducer,
        metricPage: metricPageReducer,
    }
});

export default store;