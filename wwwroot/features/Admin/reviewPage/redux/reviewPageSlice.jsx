import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_REVIEWS_URL } from '../../../../apiConfig';
import { notifySuccess } from '../../../notify';

export const fetchReviews = createAsyncThunk(
    'reviews/fetchReviews',
    async ({ pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query }) => {

        const response = await axios.get(`${API_REVIEWS_URL}/reviews/${pageNumber}/${pageSize}`, {
            params: { sortOrder, period: timePeriodFilter, filter: filter, query: query },
        });
        return response.data;
    }
);

export const removeReview = createAsyncThunk(
    'reviews/removeReview',
    async (reviewId) => {
        await axios.delete(`${API_REVIEWS_URL}/admin/review/${reviewId}`);
        notifySuccess('Отзыв успешно удалён');
    }
);

const initialState = {  
    pageNumber: 1,
    pageSize: 30,
    query: '',
    timePeriodFilter: null,
    filter: null,
    sortOrder: null,
    totalReviews: 0,
    totalPages: 0,
    reviews: [],
    isLoaded: false,
    isPageReset: false
};

const reviewPageSlice = createSlice({
    name: 'reviewPage',
    initialState,
    reducers: {
        setQuery: (state, action) => {
            state.query = action.payload;
        },
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
        setFilter(state, action) {
            state.filter = action.payload;
        },
        setIsPageReset(state, action) {
            state.isPageReset = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchReviews.fulfilled, (state, action) => {
                const { items, totalItems, totalPages } = action.payload;
                state.reviews = items;
                state.totalReviews = totalItems;
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
    setFilter,
    setIsPageReset,
} = reviewPageSlice.actions;

export default reviewPageSlice.reducer;