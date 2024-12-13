import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { notifySuccess } from '../../notify';
import { API_REVIEWS_URL } from '../../../apiConfig';

export const fethAllReviewsInfo = createAsyncThunk(
    'reviews/fethAllReviewsInfo',
    async (productId, { dispatch }) => {
        console.log("Отработал ФЕТЧ ВСЕГО");
        await dispatch(fetchProductReviews(productId));
        await dispatch(fetchCurrentReview(productId));
        await dispatch(fetchAvgProductRate(productId));
        await dispatch(fetchReviewsCount(productId));
    }
);


export const fetchProductReviews = createAsyncThunk(
    'reviews/fetchProductReviews',
    async (productId) => {
        const response = await axios.get(`${API_REVIEWS_URL}/reviews/${productId}`);
        return response.data;
    }
);

export const fetchCurrentReview = createAsyncThunk(
    'reviews/fetchCurrentReview',
    async (productId) => {
        const response = await axios.get(`${API_REVIEWS_URL}/user-review/${productId}`);
        return response.data;
    }
);

export const fetchCurrentUserId = createAsyncThunk(
    'reviews/fetchCurrentUserId',
    async () => {
        const response = await axios.get(`${API_REVIEWS_URL}/userId`);
        return response.data;
    }
);


export const fetchReviewsCount = createAsyncThunk(
    'reviews/fetchReviewsCount',
    async (productId) => {
        const response = await axios.get(`${API_REVIEWS_URL}/count-reviews/${productId}`);
        return response.data;
    }
);

export const fetchAvgProductRate = createAsyncThunk(
    'reviews/fetchAvgProductRate',
    async (productId) => {
        const response = await axios.get(`${API_REVIEWS_URL}/avg-rate/${productId}`);
        return response.data;
    }
);


export const addReview = createAsyncThunk(
    'reviews/addReview',
    async (reviewDto, { dispatch }) => {
        await axios.post(`${API_REVIEWS_URL}/review`,reviewDto);
        dispatch(fethAllReviewsInfo(reviewDto.productId)); 
        notifySuccess('Спасибо за отзыв!');
    }
);

export const updateReview = createAsyncThunk(
    'reviews/updateReview',
    async (reviewDto, { dispatch }) => {
        await axios.put(`${API_REVIEWS_URL}/review`, reviewDto);
        dispatch(fethAllReviewsInfo(reviewDto.productId));
        notifySuccess('Отзыв успешно изменён');
    }
);

export const removeReview = createAsyncThunk(
    'reviews/removeReview',
    async (productId, { dispatch }) => {
        await axios.delete(`${API_REVIEWS_URL}/review/${productId}`);
        dispatch(fethAllReviewsInfo(productId)); 
        notifySuccess('Отзыв успешно удалён');
    }
);

const reviewSlice = createSlice({
    name: 'reviews',
    initialState: {
        productId: 0,
        countReviews: 0,
        productReviews: [],
        avgReviewRate: 0,
        currentUserId: '',
        review: null,
        isLoaded: false,
    },
    reducers: {
        setProductId: (state, action) => {
            state.productId = action.payload;
            state.isLoaded = true;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchProductReviews.fulfilled, (state, action) => {
                state.productReviews = action.payload;
            })
            .addCase(fetchCurrentReview.fulfilled, (state, action) => {
                state.review = action.payload || null;
            })
            .addCase(fetchCurrentUserId.fulfilled, (state, action) => {
                state.currentUserId = action.payload;
            })
            .addCase(fetchReviewsCount.fulfilled, (state, action) => {
                state.countReviews = action.payload;
            })
            .addCase(fetchAvgProductRate.fulfilled, (state, action) => {
                state.avgReviewRate = action.payload;
            })
    },
});

export const { setProductId } = reviewSlice.actions;
export default reviewSlice.reducer;