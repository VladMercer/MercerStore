import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_ORDERS_URL } from '../../../../apiConfig';

export const fetchOrders = createAsyncThunk(
    'orders/fetchOrders',
    async ({ pageNumber, pageSize, sortOrder, timePeriodFilter, statusFilter, query }) => {

        const response = await axios.get(`${API_ORDERS_URL}/orders/${pageNumber}/${pageSize}`, {
            params: { sortOrder, period: timePeriodFilter, status: statusFilter, query },
        });
        return response.data;
    }
);

const initialState = {
    pageNumber: 1,
    pageSize: 30,
    timePeriodFilter: null,
    statusFilter: null,
    sortOrder: null,
    totalOrders: 0,
    totalPages: 0,
    orders: [],
    query: '',
    isLoaded: false,
    isPageReset: false
};

const orderPageSlice = createSlice({
    name: 'orderPage',
    initialState,
    reducers: {
        setPageNumber: (state, action) => {
            state.pageNumber = action.payload;
        },
        setPageSize(state, action) {
            state.pageSize = action.payload;
        },
        setSortOrder(state, action) {
            state.sortOrder = action.payload;
        },
        setTimePeriodFilter(state, action) {
            state.timePeriodFilter = action.payload;
        },
        setStatusFilter(state, action) {
            state.statusFilter = action.payload;
        },
        setIsPageReset(state, action) {
            state.isPageReset = action.payload;
        },
        setQuery(state, action) {
            state.query = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchOrders.fulfilled, (state, action) => {
                const { items, totalItems, totalPages } = action.payload;
                state.orders = items;
                state.totalOrders = totalItems;
                state.totalPages = totalPages;
                state.isLoaded = true;
            })
    },
});

export const {
    setQuery,
    setPageNumber,
    setPageSize,
    setSortOrder,
    setTimePeriodFilter,
    setStatusFilter,
    setIsPageReset,
} = orderPageSlice.actions;

export default orderPageSlice.reducer;