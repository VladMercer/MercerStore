import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { notifySuccess } from '../../notify';
import { API_CARTS_URL } from '../../../apiConfig';


export const fetchCartData = createAsyncThunk('cart/fetchCartData', async () => {
    const response = await axios.get(`${API_CARTS_URL}/products`);
    return response.data;
});


export const addToCart = createAsyncThunk('cart/addToCart', async (productId, { dispatch }) => {
    await axios.post(`${API_CARTS_URL}/product/${productId}`);
    dispatch(fetchCartData());
    notifySuccess('Товар успешно добавлен в корзину!');
    return productId;
});

export const removeFromCart = createAsyncThunk('cart/removeFromCart', async (productId, { dispatch }) => {
    await axios.delete(`${API_CARTS_URL}/product/${productId}`);
    dispatch(fetchCartData());
    notifySuccess('Товар успешно удален из корзины!');
    return productId;
});

const cartSlice = createSlice({
    name: 'cart',
    initialState: {
        items: [],
        totalPrice: 0,
        itemCount: 0,
        status: 'idle',
        error: null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchCartData.fulfilled, (state, action) => {
                state.items = action.payload.cartItems;
                state.totalPrice = action.payload.cartTotalPrice;
                state.itemCount = action.payload.cartItemCount;
                state.isLoaded = true;
            })
        
    },
});

export default cartSlice.reducer;