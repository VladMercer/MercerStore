import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import axios from 'axios';
import {API_CATEGORIES_URL} from '../../../apiConfig';

export const fetchPriceRange = createAsyncThunk(
    'category/fetchPriceRange',
    async (categoryId) => {
        const response = await axios.get(`${API_CATEGORIES_URL}/price-range/${categoryId}`);
        return response.data;

    }
);

export const fetchProducts = createAsyncThunk(
    'category/fetchProducts',
    async ({categoryId, pageNumber, pageSize, sortOrder, minPrice, maxPrice}) => {
        const page = pageNumber || 1;
        const size = pageSize || 9;

        const response = await axios.get(`${API_CATEGORIES_URL}/products`, {
            params: {
                categoryId,
                pageNumber: page,
                pageSize: size,
                sortOrder,
                minPrice,
                maxPrice
            }
        });

        return response.data;
    }
);

const initialState = {
    categoryId: 0,
    pageNumber: 1,
    pageSize: 9,
    sortOrder: '',
    totalProducts: 0,
    totalPages: 0,
    products: [],
    minPrice: 0,
    maxPrice: 1000000,
    selectedMinPrice: 0,
    selectedMaxPrice: 1000000,
    status: 'idle',
    error: null,
    isLoaded: false,
    isPriceRangeLoaded: false,
    isPageReset: false
};

const categorySlice = createSlice({
    name: 'category',
    initialState,
    reducers: {
        setCategoryId(state, action) {
            state.categoryId = action.payload;
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
        setSelectedMinPrice(state, action) {
            state.selectedMinPrice = action.payload;
        },
        setSelectedMaxPrice(state, action) {
            state.selectedMaxPrice = action.payload;
        },
        setIsPageReset(state, action) {
            state.isPageReset = action.payload;
        },


    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchPriceRange.fulfilled, (state, action) => {
                const {minPrice, maxPrice} = action.payload;
                state.minPrice = minPrice;
                state.maxPrice = maxPrice;
                state.selectedMinPrice = minPrice;
                state.selectedMaxPrice = maxPrice;
                state.isPriceRangeLoaded = true;
            })
            .addCase(fetchProducts.fulfilled, (state, action) => {
                const {items, totalItems, totalPages} = action.payload;
                state.products = items;
                state.totalProducts = totalItems;
                state.totalPages = totalPages;
                state.isLoaded = true;
            });
    },
});

export const {
    setCategoryId,
    setPageNumber,
    setPageSize,
    setSortOrder,
    setSelectedMinPrice,
    setSelectedMaxPrice,
    setIsPageReset
} = categorySlice.actions;

export default categorySlice.reducer;