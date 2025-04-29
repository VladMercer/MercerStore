import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import axios from 'axios';
import {API_USERS_URL} from '../../../../apiConfig';

export const fetchUsers = createAsyncThunk(
    'users/fetchUsers',
    async ({pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query}) => {

        const response = await axios.get(`${API_USERS_URL}/users`, {
            params: {pageNumber, pageSize, sortOrder, period: timePeriodFilter, filter: filter, query: query},
        });
        return response.data;
    }
);

const initialState = {
    pageNumber: 1,
    pageSize: 30,
    query: '',
    timePeriodFilter: null,
    filter: null,
    sortOrder: null,
    totalUsers: 0,
    totalPages: 0,
    users: [],
    isLoaded: false,
    isPageReset: false
};

const userPageSlice = createSlice({
    name: 'userPage',
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
            .addCase(fetchUsers.fulfilled, (state, action) => {
                const {items, totalItems, totalPages} = action.payload;
                state.users = items;
                state.totalUsers = totalItems;
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
} = userPageSlice.actions;

export default userPageSlice.reducer;