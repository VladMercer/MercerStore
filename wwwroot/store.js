import { configureStore } from '@reduxjs/toolkit';
import cartReducer from './features/cart/redux/cartSlice';
import categoryReducer from './features/categoryPage/redux/categorySlice';
import searchPageReducer from './features/searchPage/redux/searchPageSlice';
import reviewsReducer from './features/reviews/redux/reviewSlice';

const store = configureStore({
    reducer: {
        cart: cartReducer,
        category: categoryReducer,
        searchPage: searchPageReducer,
        reviews: reviewsReducer
    }
});

export default store;