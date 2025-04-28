import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import axios from 'axios';
import {API_METRICS_URL} from '../../../../apiConfig';

export const fetchMetrics = createAsyncThunk(
    'metrics/fetchMetrics',
    async () => {
        const response = await axios.get(`${API_METRICS_URL}/metrics`);
        return response.data;
    }
);


const initialState = {
    sales: {
        daily: 0,
        weekly: 0,
        monthly: 0,
        yearly: 0,
        totalOrders: {daily: 0, weekly: 0, monthly: 0, yearly: 0},
        averageOrderValue: 0,
        topProducts: [],
    },
    reviews: {
        total: 0,
        averageRating: 0,
        newReviews: {daily: 0, weekly: 0, monthly: 0},
        topRatedProducts: [],
    },
    users: {
        total: 0,
        newUsers: {daily: 0, weekly: 0, monthly: 0, yearly: 0},
    },
    suppliers: {total: 0},
    invoices: {
        daily: 0,
        weekly: 0,
        monthly: 0,
        yearly: 0,
        totalInvoices: {daily: 0, weekly: 0, monthly: 0, yearly: 0},
        averageInvoiceValue: 0,
        topProducts: [],
    },
    isLoaded: false,
};

const metricPageSlice = createSlice({
    name: 'metricPage',
    initialState,
    reducer: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchMetrics.fulfilled, (state, action) => {
                state.sales = action.payload.sales;
                state.reviews = action.payload.reviews;
                state.users = action.payload.users;
                state.suppliers = action.payload.suppliers;
                state.invoices = action.payload.invoices;
                state.isLoaded = true;
            })
    },
});

export default metricPageSlice.reducer;