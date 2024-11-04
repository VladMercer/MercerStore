import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';

export const fetchPriceRange = createAsyncThunk(
    'category/fetchPriceRange',
    async (categoryId) => {
        const response = await fetch(`/CategoryApi/price-range?categoryId=${categoryId}`);
        return response.json();
    }
);

export const fetchProducts = createAsyncThunk(
    'category/fetchProducts',
    async ({ categoryId, pageNumber, pageSize, sortOrder, minPrice, maxPrice }) => {
        const response = await fetch(`/CategoryApi/products?categoryId=${categoryId}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}&minPrice=${minPrice}&maxPrice=${maxPrice}`);
        return response.json();
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
};

const categorySlice = createSlice({
    name: 'category',
    initialState,
    reducers: {
        setPageNumber(state, action) {
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
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchPriceRange.fulfilled, (state, action) => {
                const { minPrice, maxPrice } = action.payload;
                state.minPrice = minPrice;
                state.maxPrice = maxPrice;
                state.selectedMinPrice = minPrice;
                state.selectedMaxPrice = maxPrice;
            })
            .addCase(fetchProducts.fulfilled, (state, action) => {
                const { products, totalItems, totalPages } = action.payload;
                state.products = products;
                state.totalProducts = totalItems;
                state.totalPages = totalPages;
            });
    },
});

export const { setPageNumber, setPageSize, setSortOrder, setSelectedMinPrice, setSelectedMaxPrice } = categorySlice.actions;
export default categorySlice.reducer;