import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_SEARCH_URL } from '../../../apiConfig';

export const fetchResults = createAsyncThunk(
    'search/fetchResults',
    async ({ query, sortOrder, pageNumber, pageSize }) => {

        const response = await axios.get(`${API_SEARCH_URL}/search`, {
            params: { query, sortOrder, pageNumber, pageSize },
        });
        return response.data;
    }
);

const searchPageSlice = createSlice({
    name: 'searchPage',
    initialState: {
        query: '',
        results: [],
        isLoaded: false,
        error: null,
        pageNumber: 1,
        pageSize: 9,
        totalPages: 0,
        sortOrder: '',
        totalItems:0,
        isPageReset: false
      
    },
    reducers: {
        setQuery: (state, action) => {
            state.query = action.payload;
         
        },
        setSortOrder: (state, action) => {
            state.sortOrder = action.payload;
           
        },
        setPageNumber: (state, action) => {
            state.pageNumber = action.payload;

        },
        setPageSize: (state, action) => {
            state.pageSize = action.payload;
          
        },
        setIsPageReset: (state, action) => {
            state.isPageReset = action.payload;

        },
        
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchResults.fulfilled, (state, action) => {
                const { products, totalItems, totalPages } = action.payload;
                state.results = products;
                state.totalPages = totalPages;
                state.totalItems = totalItems
                state.isLoaded = true;
            })
    },
});

export const {
    setQuery,
    setSortOrder,
    setPageNumber,
    setPageSize,
    setIsPageReset
} = searchPageSlice.actions;

export default searchPageSlice.reducer;