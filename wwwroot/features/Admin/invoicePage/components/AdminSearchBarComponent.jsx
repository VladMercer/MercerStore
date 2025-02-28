import React, { useRef } from 'react';
import { useDispatch } from 'react-redux';
import { setQuery } from '../redux/invoicePageSlice';

const AdminSearchBarComponent = () => {
    const dispatch = useDispatch();
    const debounceTimer = useRef(null);

    const handleInputChange = (e) => {
        const newQuery = e.target.value;

        if (debounceTimer.current) {
            clearTimeout(debounceTimer.current);
        }

        debounceTimer.current = setTimeout(() => {
            dispatch(setQuery(newQuery));
        }, 400);
    };

    return (
        <div className="position-relative">
            <form className="d-flex input-group" onSubmit={(e) => e.preventDefault()}>
                <input
                    type="text"
                    className="form-control"
                    onChange={handleInputChange}
                    placeholder="Поиск"
                    aria-label="Search"
                    aria-describedby="button-search"
                />
                <button className="btn btn-outline-warning" type="submit" id="button-search">
                    <i className="fa-solid fa-magnifying-glass"></i>
                </button>
            </form>
        </div>
    );
};

export default AdminSearchBarComponent;