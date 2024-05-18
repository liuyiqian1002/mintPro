module.exports = {
  'plugins': {
     
      'postcss-px2rem-exclude': {
          remUnit: 75, //750的设计稿
          exclude: /node_modules|folder_name/i 
      }
  }
}