import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import axios from 'axios';
import {API_SEARCH_URL} from '../../../../apiConfig';

export const fetchResults = createAsyncThunk(
    'search/fetchResults',
    async ({query}) => {
        const pageSize = 9;
        const pageNumber = 1;
        const response = await axios.get(`${API_SEARCH_URL}/search`, {
            params: {query, pageNumber, pageSize},
        });
        return response.data;
    }
);

const adminSearchPageSlice = createSlice({
    name: 'adminSearchPage',
    initialState: {
        query: '',
        results: [],
        pageNumber: 1,
        pageSize: 9,
    },
    reducers: {
        setQuery: (state, action) => {
            state.query = action.payload;

        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchResults.fulfilled, (state, action) => {
                const {products} = action.payload;
                state.results = products;
            })
    },
});

export const {
    setQuery,
} = adminSearchPageSlice.actions;

export default adminSearchPageSlice.reducer;