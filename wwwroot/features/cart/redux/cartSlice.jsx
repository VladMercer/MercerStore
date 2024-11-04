import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import notifySuccess from '../../notify';


export const fetchCartData = createAsyncThunk('cart/fetchCartData', async () => {
    const response = await axios.get('/CartApi/GetProducts');
    return response.data;
});


export const addToCart = createAsyncThunk('cart/addToCart', async (productId, { dispatch }) => {
    await axios.post(`/CartApi/AddToCart?quantity=1&productId=${productId}`);
    dispatch(fetchCartData());
    notifySuccess('Товар успешно добавлен в корзину!');
    return productId;
});

export const removeFromCart = createAsyncThunk('cart/removeFromCart', async (productId, { dispatch }) => {
    await axios.post(`/CartApi/RemoveFromCart?productId=${productId}`);
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