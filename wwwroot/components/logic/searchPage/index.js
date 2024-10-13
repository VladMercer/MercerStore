import ReactDOM from 'react-dom';
import ProductListApp from './ProductListApp';
import PaginationApp from './PaginationApp';
import PageSizeSelectorApp from './PageSizeSelectorApp';
import SortApp from './SortApp';
import { createPortal } from 'react-dom';

const pageSizeRoot = document.getElementById('search-page-size-root');
const paginationRoot = document.getElementById('search-pagination-root');
const productListRoot = document.getElementById('search-product-list-root');
const sortRoot = document.getElementById('search-sort-root');

const SearchAppWithPortals = () => (
            <>
                {sortRoot && createPortal(<SortApp />, sortRoot)}
                {pageSizeRoot && createPortal(<PageSizeSelectorApp />, pageSizeRoot)}
                {paginationRoot && createPortal(<PaginationApp />, paginationRoot)}
                {productListRoot && createPortal(<ProductListApp />, productListRoot)}
            </>
);
export default SearchAppWithPortals;