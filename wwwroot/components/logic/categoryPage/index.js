import React from 'react';
import ReactDOM, { createPortal } from 'react-dom';
import PageSizeSelectorApp from './PageSizeSelectorApp';
import PaginationApp from './PaginationApp';
import ProductListApp from './ProductListApp';
import SortApp from './SortApp';
import TotalProductsInfoApp from './TotalProductsInfoApp';
import FilterApp from './FilterApp';


const pageSizeRoot = document.getElementById('category-page-size-root');
const paginationRoot = document.getElementById('category-pagination-root');
const productListRoot = document.getElementById('category-product-list-root');
const sortRoot = document.getElementById('category-sort-root');
const totalProductsInfoRoot = document.getElementById('total-products-info-root');
const filterRoot = document.getElementById('filter-root')

const CategoryAppWithPortals = () => (
            <>
            {sortRoot && createPortal(<SortApp />, sortRoot)}
            {pageSizeRoot && createPortal(<PageSizeSelectorApp />, pageSizeRoot)}
            {paginationRoot && createPortal(<PaginationApp />, paginationRoot)}
            {productListRoot && createPortal(<ProductListApp />, productListRoot)}
            {totalProductsInfoRoot && createPortal(<TotalProductsInfoApp />, totalProductsInfoRoot)}
            {filterRoot && createPortal(<FilterApp />, filterRoot)}
            </>
);

export default CategoryAppWithPortals;