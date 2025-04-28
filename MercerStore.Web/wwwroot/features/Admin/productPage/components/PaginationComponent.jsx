import React from 'react';
import {useProducts} from '../hooks/useProducts';
import {useDispatch} from 'react-redux';
import {setPageNumber} from '../redux/adminProductPageSlice';

const PaginationComponent = () => {
    const {pageNumber, totalPages} = useProducts();
    const dispatch = useDispatch();

    const changePageNumber = (newPageNumber) => {
        if (newPageNumber > 0 && newPageNumber <= totalPages) {
            dispatch(setPageNumber(newPageNumber));
        }
    };

    return (
        <nav aria-label="Page navigation example">
            <ul className="pagination">
                <li className={`page-item ${pageNumber <= 1 ? 'disabled' : ''}`}>
                    <a className="page-link" href="#" onClick={() => changePageNumber(pageNumber - 1)}>
                        Назад
                    </a>
                </li>
                {[...Array(totalPages)].map((_, i) => (
                    <li key={i} className={`page-item ${pageNumber === i + 1 ? 'active' : ''}`}>
                        <a className="page-link" href="#" onClick={() => changePageNumber(i + 1)}>
                            {i + 1}
                        </a>
                    </li>
                ))}
                <li className={`page-item ${pageNumber >= totalPages ? 'disabled' : ''}`}>
                    <a className="page-link" href="#" onClick={() => changePageNumber(pageNumber + 1)}>
                        Вперёд
                    </a>
                </li>
            </ul>
        </nav>
    );
};

export default PaginationComponent;