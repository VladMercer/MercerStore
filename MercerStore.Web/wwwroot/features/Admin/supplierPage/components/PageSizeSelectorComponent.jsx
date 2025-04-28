import {useDispatch} from 'react-redux';
import React from 'react';
import {useSuppliers} from '../hooks/useSuppliers';
import {setPageSize} from '../redux/supplierPageSlice';

const PageSizeSelectorComponent = () => {
    const {pageSize} = useSuppliers();
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
                <option value="30">30</option>
                <option value="45">45</option>
                <option value="60">60</option>
                <option value="90">90</option>
                <option value="120">120</option>
                <option value="150">150</option>
            </select>
        </div>
    );
};

export default PageSizeSelectorComponent;