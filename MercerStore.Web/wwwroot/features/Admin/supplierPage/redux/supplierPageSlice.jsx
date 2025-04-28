import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import axios from 'axios';
import {API_SUPPLIERS_URL} from '../../../../apiConfig';
import {notifySuccess} from '../../../notify';

export const fetchSuppliers = createAsyncThunk(
    'users/fetchSuppliers',
    async ({pageNumber, pageSize, query}) => {

        const response = await axios.get(`${API_SUPPLIERS_URL}/suppliers`, {
            params: {pageNumber, pageSize, query: query},
        });
        return response.data;
    }
);

export const removeReview = createAsyncThunk(
    'suppliers/removeSupplier',
    async (supplierId) => {
        await axios.delete(`${API_SUPPLIERS_URL}/supplier/${supplierId}`);
        notifySuccess('Поставщик успешно удалён');
    }
);

const initialState = {
    pageNumber: 1,
    pageSize: 30,
    query: '',
    totalSuppliers: 0,
    totalPages: 0,
    suppliers: [],
    isLoaded: false,
    isPageReset: false
};

const supplierPageSlice = createSlice({
    name: 'supplierPage',
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
        setIsPageReset(state, action) {
            state.isPageReset = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchSuppliers.fulfilled, (state, action) => {
                const {items, totalItems, totalPages} = action.payload;
                state.suppliers = items;
                state.totalSuppliers = totalItems;
                state.totalPages = totalPages;
                state.isLoaded = true;
            })
    },
});

export const {
    setQuery,
    setPageNumber,
    setPageSize,
    setIsPageReset,
} = supplierPageSlice.actions;

export default supplierPageSlice.reducer;