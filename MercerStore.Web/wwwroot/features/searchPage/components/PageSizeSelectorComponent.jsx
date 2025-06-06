﻿import {useDispatch} from 'react-redux';
import React from 'react';
import {useSearchPage} from '../hooks/useSearchPage';
import {setPageSize} from '../redux/searchPageSlice';

const PageSizeSelectorComponent = () => {
    const {pageSize} = useSearchPage();
    const dispatch = useDispatch();

    const changePageSize = (newPageSize) => {
        dispatch(setPageSize(newPageSize));
    };

    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value, 10);
        changePageSize(newSize);
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Показывать:</span>
            <select className="form-select" value={pageSize} onChange={handlePageSizeChange}>
                <option value="9">9</option>
                <option value="15">15</option>
                <option value="30">30</option>
                <option value="45">45</option>
            </select>
        </div>
    );
};

export default PageSizeSelectorComponent;