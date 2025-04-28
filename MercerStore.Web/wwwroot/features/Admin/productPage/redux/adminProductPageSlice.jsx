import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import axios from 'axios';
import {API_CATEGORIES_URL, API_PRODUCTS_URL} from '../../../../apiConfig';


export const fetchCategories = createAsyncThunk(
    'categories/fetchCategories',
    async () => {
        const response = await axios.get(`${API_CATEGORIES_URL}/categories`);
        return response.data;
    }
);

export const fetchProducts = createAsyncThunk(
    'products/fetchProducts',
    async ({categoryId, pageNumber, pageSize, sortOrder, filter}) => {

        const response = await axios.get(`${API_PRODUCTS_URL}/products/`, {
            params: {pageNumber, pageSize, categoryId, sortOrder, filter}
        });
        return response.data;
    }
);

const initialState = {
    categories: [],
    categoryId: null,
    pageNumber: 1,
    pageSize: 30,
    filter: null,
    sortOrder: null,
    totalProducts: 0,
    totalPages: 0,
    products: [],
    productStatus: '',
    isLoaded: false,
    isPageReset: false
};

const adminProductPageSlice = createSlice({
    name: 'adminProductPage',
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
        setFilter(state, action) {
            state.filter = action.payload;
        },
        setIsPageReset(state, action) {
            state.isPageReset = action.payload;
        },
        setCategories(state, action) {
            state.isPageReset = action.payload;
        },


    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchProducts.fulfilled, (state, action) => {
                const {items, totalItems, totalPages} = action.payload;
                state.products = items;
                state.totalProducts = totalItems;
                state.totalPages = totalPages;
                state.isLoaded = true;
            })
            .addCase(fetchCategories.fulfilled, (state, action) => {
                state.categories = action.payload;
            });
    },
});

export const {
    setCategoryId,
    setPageNumber,
    setPageSize,
    setSortOrder,
    setFilter,
    setIsPageReset,
    setCategories
} = adminProductPageSlice.actions;

export default adminProductPageSlice.reducer;